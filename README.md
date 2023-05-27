# Using Hangfire and SendGrid to Send asynchronous emails

## Description
This project demonstrates how to use Hangfire and SendGrid to send newsletter emails in the background at a 2-minute interval.

## Features
- Background task processing using Hangfire
- Sending emails using SendGrid
- Configurable email interval

## Technologies
- Hangfire
- SendGrid

## Installation
1. Clone the repository: `git clone https://github.com/Matthew-Oduamafu/HangfireAndSendGrid.git`
2. Navigate to the project directory: `cd HangfireAndSendGrid`
3. Install dependencies: `dotnet restore`
4. Configure SendGrid API key: 
   - Open `appsettings.json` file
   - Replace `YOUR_SENDGRID_API_KEY` with your actual SendGrid API key or Add ApiKey to Environment variables
5. Build the project: `dotnet build`

## Usage
1. Run the project: `dotnet run`
2. Access the application at: `https://localhost:7256`
3. The newsletter emails will be sent automatically in the background at a 2-minute interval.

## Configuration
- `appsettings.json`: Contains configuration settings for Hangfire and SendGrid.

<!-- ## License
This project is licensed under the [MIT License](LICENSE). -->

