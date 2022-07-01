## Hints

### SingleFile Publishing

- When publishing to singlefile, config files should be marked with `<ExcludeFromSingleFile>true</ExcludeFromSingleFile>` in the project file to ensure you can still modify them on the target system.


### Installation on Linux

- copy file `Gasprices.service` to the `/etc/systemd/system/`folder 
- start the service: `systemctl start Gasprices`
- switch autostart for the service on: `systemctl enable Gasprices` 