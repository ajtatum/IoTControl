Welcome to Hacker101!
=====================

IoTControl is an ASP.NET MVC 5 application to control Internet of Things (IoT) devices. This is a personal project that I work on to play around with IoT device APIs; however, it works "in the wild."

The application currently works with LIFX.

Getting Started
---------------

You'll need to add two files:

1. IoTControl.Web/appSettings.config
2. IoTControl.Web/connectionStrings.config

In appSettings.config, you'll have:

```<language>
<?xml version="1.0" encoding="utf-8"?>
<appSettings>
  <add key="GoogleClientId" value=""/>
  <add key="GoogleClientSecret" value=""/>
</appSettings>
```

And in connectionStrings.config:

```<language>
<?xml version="1.0" encoding="utf-8"?>
<connectionStrings>
  <add name="DefaultConnection" connectionString="" providerName="System.Data.SqlClient" />
</connectionStrings>
```

To Do
---------------

- Currently, for LIFX integration we ask users to provide an their access token. Instead, we should utilize LIFX's OAuth integration.
- Work can be done to improve the separation of layers between this application and IoT APIs.
- Unit & Integration Testing
- and more!