# Email API

This project is an Email Sender API that uses an SMTP server to send emails to specified recipients and includes options for authentication. It leverages Azure services for configuration and secrets management.

## Key Features
- **Email Sending**: Sends emails via an SMTP server.
- **Authentication**: Supports specifying an authentication key for API access.
- **CI/CD Pipeline**: Automated deployment to Azure App Service and API Management.
- **Configuration & Secrets Management**: Utilizes Azure App Configuration and Key Vaults.
- **Testing**: Includes unit and integration tests.

## Getting Started

### Prerequisites
- SMTP server credentials
- Azure account (for production)

### Steps to Run the Project
1. **Clone the Repository**:
    ```bash
    git clone https://github.com/Sane7222/Email-API.git
    ```
2. **Configure SMTP Server Credentials**:
    - Update the app settings with your SMTP server credentials, and optionally authentication token.
3. **Start the Project**:
    ```bash
    dotnet run
    ```
4. **Send Requests to the API**:
    - Use the `EmailAPI.http` file to send requests.

## Running Tests
- The project includes both unit and integration tests located in the `EmailAPITests` folder.
- To run the tests:
    ```bash
    dotnet test
    ```

## Contributing
Contributions are welcome! Please fork the repository and submit a pull request.

## License
This project is licensed under the MIT License.
