# Toolsfactory.Home.Adapters
A set of different adapters that translate information from external subsystems like prices from gas stations into homie standard based mqtt messages consumable by many different home automation platforms


## Docker configuration

### Environment variables
* MqttServer__Address="192.168.2.50" -  local mqtt server ip
* MqttServer__Username="somename" - username to log into the mqtt server
* MqttServer__Password="somepwd" - password to log into the mqtt server
* services__gasprices__Tankerkoenig__ApiKey="api key" - personal api key for the tankerkoenig service

### Volumes
* /etc/homie-multihost/ Should be mapped to an external folder containing the appsettings.*.json files. (Readonly)
* /var/log/toolsfactory/ used to store logs (Read/Write)

### Ports
The docker image exposes the following ports:
* 12004: Used by the heating server for KNX Object Server messages. Must not be changed via appsettings.json!
* 8081: USed by the weather server to receive calls from the WeatherStation system. Must not be changed via appsettings.json!
