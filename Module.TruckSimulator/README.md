# Truck Simulator Plugin

This plugin adds a profile module for Euro Truck Simulator 2 and American Truck Simulator.

A data model is included which fetches data from the game via memory-mapped files using the SDK from [https://github.com/RenCloud/scs-sdk-plugin](https://github.com/RenCloud/scs-sdk-plugin).
To install this plugin, either use the install button in the plugin settings window in Artemis _or_ download the latest SDK release from the GitHub link and manually copy the 32-bit or 64-bit dll from the download to the `<GAME DIRECTORY>/bin/<ARCH>/plugins` directory (you may need to create `plugins` it if it does not exist).

The data model includes data such as:
- Speed
- Engine RPM
- Indicators
- Fuel
- Air pressure
- Remaining time on job
- Trailers (and whether they are attached)
- And much more.
