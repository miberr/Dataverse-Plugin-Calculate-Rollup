using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System;
using System.Linq;

namespace Plugin
{
    /// <summary>
    /// Calculate Rollup type field manually
    /// </summary>
    public class CalculateRollup : PluginBase
    {
        private string _rollupColumnName;
        private string _lookupColumnName;

        private readonly string _registerHelp = "Please register the rollup entity, rollup column name and local lookup column name into unsecure configuration with format rollupColumnName;lookupColumnName.";

        public CalculateRollup(string unsecureConfiguration, string secureConfiguration)
            : base(typeof(CalculateRollup))
        {
            // Throw error if there is no configuration
            if (unsecureConfiguration == null)
                throw new ArgumentNullException($"No configuration registered. {this._registerHelp}");

            try
            {
                string[] configurations = unsecureConfiguration.Split(';');
                this._rollupColumnName = configurations[0];
                this._lookupColumnName = configurations[1];
            }
            catch
            {
                throw new ArgumentException($"Argument formatting in correct. {this._registerHelp}");
            }

        }

        // Entry point for custom business logic execution
        protected override void ExecuteDataversePlugin(ILocalPluginContext localPluginContext)
        {
            try
            {
                if (localPluginContext == null)
                {
                    throw new ArgumentNullException(nameof(localPluginContext));
                }

                var context = localPluginContext.PluginExecutionContext;

                // Switch on message name
                switch (context.MessageName)
                {
                    case "Create":

                        RecalculateRollup(GetEntityImage(context.PostEntityImages), localPluginContext);
                        break;

                    case "Update":

                        RecalculateRollup(GetEntityImage(context.PreEntityImages), localPluginContext);
                        RecalculateRollup(GetEntityImage(context.PostEntityImages), localPluginContext);
                        break;

                    case "Delete":

                        RecalculateRollup(GetEntityImage(context.PreEntityImages), localPluginContext);
                        break;

                    default:

                        throw new ArgumentOutOfRangeException($"Message of type {localPluginContext.PluginExecutionContext.MessageName} is not supported by this plugin.");
                }

            }
            catch (Exception ex)
            {
                // Log the exeption
                localPluginContext.Trace($"Exception: {ex.ToString()}");
            }


        }

        private Entity GetEntityImage(EntityImageCollection imageCollection)
        {
            if (imageCollection.Count > 0)
                return imageCollection.Values.First();
            else
                throw new ArgumentNullException($"No entity image found. Register image depending on message. Create: Post, Update: Pre and Post, Delete: Pre.");
        }

        private void RecalculateRollup(Entity entity, ILocalPluginContext localPluginContext)
        {
            var orgService = localPluginContext.InitiatingUserService;

            // Check if lookup has value
            if (entity.TryGetAttributeValue(this._lookupColumnName, out EntityReference entityReference))
            {
                localPluginContext.Trace($"Trying to calculate rollup {this._rollupColumnName} for {entityReference.LogicalName} {entityReference.Id}.");

                // Create request
                CalculateRollupFieldRequest request = new CalculateRollupFieldRequest();
                request.FieldName = this._rollupColumnName;
                request.Target = entityReference;

                // Send request
                orgService.Execute(request);

                localPluginContext.Trace($"Rollup {this._rollupColumnName} for {entityReference.LogicalName} {entityReference.Id} was successfully recalculated.");
            }
            else
            {
                // Check if field exists
                RetrieveAttributeRequest retrieveAttributeRequest = new RetrieveAttributeRequest
                {
                    EntityLogicalName = entity.LogicalName,
                    LogicalName = this._lookupColumnName
                };

                RetrieveAttributeResponse retrieveAttributeResponse = (RetrieveAttributeResponse)orgService.Execute(retrieveAttributeRequest);

                // If no error was thrown, check the type is lookup-
                if (retrieveAttributeResponse.AttributeMetadata.AttributeType == Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.Lookup)
                {
                    localPluginContext.Trace($"Lookup {this._lookupColumnName} for {entity.LogicalName} {entity.Id} is empty and no calculation was triggered.");
                }
                else
                {
                    throw new ArgumentException($"Column {this._lookupColumnName} for {entity.LogicalName} is not of type Lookup.");
                }
            }
        }
    }
}
