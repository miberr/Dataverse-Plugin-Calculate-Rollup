# Dataverse Plugin for Real-Time Rollup Calculations

This repository contains a reusable Dataverse plugin that calculates rollup fields in real time. By leveraging the power of Dataverse plugins, this solution ensures that business logic is executed efficiently and reliably, solving the problem of delayed rollup calculations.

## Features

-   Real-time rollup calculations for Dataverse entities
-   Reusable plugin that can be applied to various scenarios
-   Configurable rollup and lookup column names
-   Handles create, update, and delete events

## Getting Started

### Prerequisites

-   Dataverse environment

### Installation

1. Clone the repository:

    ```bash
    git clone https://github.com/miberr/Dataverse-Plugin-Calculate-Rollup.git
    cd Dataverse-Plugin-Calculate-Rollup
    ```

2. Open the project in Visual Studio 2022.

3. Build the solution:

    - Click on **Build** and then **Build Solution**.

4. Register the plugin assembly and steps using the Plugin Registration Tool:
    - Open your terminal and run the following command:
        ```bash
        pac tool prt
        ```
    - Connect to your Dataverse environment.
    - Register the assembly file located in `bin\Debug\net462\Miberr.CalculateRollup.dll`.
    - Register the necessary steps and images as described in the blog post.

### Configuration

When registering the plugin steps, provide the following configuration string in the **Unsecure Configuration** field:

-   `rollupColumnName`: The logical name of the rollup column.
-   `lookupColumnName`: The logical name of the lookup column.

### Example

For example, to register a step for the `Create` message on the `mb_shift` entity with the rollup column `mb_totalhours` and the lookup column `mb_employee`, use the following configuration string:

## Usage

Once the plugin is registered and configured, it will automatically calculate the rollup field in real time whenever a related entity is created, updated, or deleted.

## Contributing

Contributions are welcome! If you find any issues or have suggestions for improvements, please open an issue or submit a pull request.

## Acknowledgements

This plugin was created as part of a blog post on creating reusable plugins in Dataverse for real-time rollup calculations. You can read the full post [here](https://github.com/miberr/Dataverse-Plugin-Calculate-Rollup).
