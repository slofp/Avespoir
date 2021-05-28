# Avespoir

[![Build Status](https://travis-ci.com/Fairy-Phy/Avespoir.svg?branch=master)](https://travis-ci.com/Fairy-Phy/Avespoir)
[![CodeFactor](https://www.codefactor.io/repository/github/fairy-phy/avespoir/badge)](https://www.codefactor.io/repository/github/fairy-phy/avespoir)
[![Apache 2.0 License](https://img.shields.io/badge/License-Apache%202.0-red.svg)](https://github.com/Fairy-Phy/Avespoir/blob/master/LICENSE)

Invite control Discord Bot

Discordbot made in C# based on [Silence](https://github.com/Fairy-Phy/Silence)

***

Avespoir was made for Undefined server, it manages server access in a whitelist format.

Therefore it is not made for public bots.

## How to build(publish) and run

### **Prerequisites**

* [.NET Core SDK](https://dotnet.microsoft.com/download) (version 3.1 or higher)

### **Step. 1**

Clone Avespoir repository.

```sh
user@pc:/ $ git clone https://github.com/Fairy-Phy/Avespoir.git
```

### **Step. 2**

Build Avespoir.

```sh
user@pc:/ $ cd Avespoir
dotnet restore

# if build Debug...
dotnet build -c Debug

# if publish Release...
dotnet publish -c Release
```

The built files will be in the ``Builds/netcoreapp3.1`` folder by default.

If you publish the files, they will be placed in the ``Builds/netcoreapp3.1/publish`` folder by default.

### **Step. 3**

Create ``ClientConfig.json`` and ``DBConfig.json``. Refer to ``ClientConfig_Template.json`` and ``DBConfig_Template.json`` for necessary items.

### **! Attention !**

If you build with Debug, it is assumed to start from the ``Builds/netcoreapp3.1`` folder.

For example, if binary folder is in ``/AvespoirRepo/Builds/netcoreapp3.1/...``, config folder should be in ``/AvespoirRepo/Configs/...``.

If you build with Release, the reference will be from the program.

```sh
# if Debug
user@pc:/Avespoir $ cd Configs
# Create ClientConfig.json and DBConfig.json

# if Release
user@pc:/Avespoir $ cd Builds/netcoreapp3.1/publish
mkdir Configs
cd Configs
# Create ClientConfig.json and DBConfig.json
```

### **Step. 4**

Run Avespoir

```sh
user@pc:/..../Configs cd ../
dotnet Avespoir.dll
# Alpha version is AvespoirTest.Test.dll
```

## How to use Docker

### **Prerequisites**

* Docker Engine (version 19.03.12? or higher)

* Docker Compose (version 1.27.0 or higher)

### **Step. 1**

Clone Avespoir repository.

```sh
user@pc:/ $ git clone https://github.com/Fairy-Phy/Avespoir.git
```

### **Step. 2**

Run docker

```sh
user@pc:/ $ cd Avespoir
docker-compose up -d
```

However, I can't start it as it is because there is no Config. So when you start it, a file will be generated in the ``Configs`` folder and you can edit it before starting it.

## Licence
Avespoir is licensed under the Apache License 2.0. Please see the licence file for more information.

-> https://github.com/Fairy-Phy/Avespoir/blob/master/LICENSE