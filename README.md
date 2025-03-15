# Dataverse Plugin for Real-Time Rollup Calculations

This repository contains a reusable Dataverse plugin that calculates rollup fields in real time. By leveraging the power of Dataverse plugins, this solution ensures that business logic is executed efficiently and reliably, solving the problem of delayed rollup calculations.

## Features

-   Real-time rollup calculations for Dataverse entities
-   Reusable
-   Handles create, update, and delete events

## Getting Started

### Installation

Register the plugin assembly and steps using the Plugin Registration Tool.

1. Connect to your Dataverse environment.
1. Register the assembly file you've downloaded.
1. Register the necessary steps and images as described below.

### Configuration

| Message | Pre Image needed | Post Image needed |
| ------- | :--------------: | :---------------: |
| Create  |       ⭕️        |        ✅         |
| Update  |        ✅        |        ✅         |
| Delete  |        ✅        |        ⭕️        |

When registering the plugin steps, provide the following configuration string in the **Unsecure Configuration** field:

`rollupColumnName;lookupColumnName`

-   `rollupColumnName`: The logical name of the rollup column.
-   `lookupColumnName`: The logical name of the lookup column.

### Example

For example, to register a step for the `Create` message on the `mb_shift` entity with the rollup column `mb_totalhours` and the lookup column `mb_employee`, use the following configuration string:

`mb_totalhours;mb_employee`

## Usage

Once the plugin is registered and configured, it will automatically calculate the rollup field in real time whenever a related entity is created, updated, or deleted.

## Contributing

Contributions are welcome! If you find any issues or have suggestions for improvements, please open an issue or submit a pull request.

## Acknowledgements

This plugin was created as part of a blog post on creating reusable plugins in Dataverse for real-time rollup calculations. You can read the full post [here](https://mikkoberg.com/blog/real-time-rollup-calculations-with-dataverse-plugins).
