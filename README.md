# .NET systemd Service Template

Installing systemd services can be a hassle. Why not let services install themselves?

This is essentially the .NET 6 `dotnet new worker` template with systemd enabled and a new `install` command added.

### Get Started

1. Build this for Linux however you like. I like: `dotnet publish --configuration Release --runtime linux-x64 --self-contained true -p:PublishSingleFile=true --output publish/`
2. Copy the executable over to Linux. ex: `multipass transfer .\publish\SystemdTemplate latest:bin/SystemdTemplate`
3. Install with the `install` command: `sudo SystemdTemplate install`

### To Do

- [ ] Add uninstall command
- [ ] Consider making this a "real" template (GitHub template repo? `dotnet add` template?)