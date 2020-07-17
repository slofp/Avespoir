# Avespoir

[![Build Status](https://travis-ci.com/Avespoir-Project/Avespoir.svg?branch=master)](https://travis-ci.com/Avespoir-Project/Avespoir)
[![CodeFactor](https://www.codefactor.io/repository/github/avespoir-project/avespoir/badge/master)](https://www.codefactor.io/repository/github/avespoir-project/avespoir/overview/master)
[![Apache 2.0 License](https://img.shields.io/badge/License-Apache%202.0-red.svg)](https://github.com/Avespoir-Project/Avespoir/blob/master/LICENSE)

Discordbot made in C# based on [Silence](https://github.com/Fairy-Phy/Silence)

***

Avespoir was made for Undefined server, it manages server access in a whitelist format.

Therefore it is not made for public bots.

## Usage

### **Prerequisites**

* [.NET Core SDK](https://dotnet.microsoft.com/download) (version 3.1 or higher)
* [MongoDB](https://www.mongodb.com/download-center/community) (latest version recommended)

Please set up the database in advance.

### **Step. 1**

Clone Avespoir repository.

```
git clone https://github.com/Avespoir-Project/Avespoir.git
```

### **Step. 2**

Build Avespoir.

```
cd Avespoir
dotnet restore
dotnet build
```

### **Step. 3**

Create ClientConfig.json and DBConfig.json. Refer to ClientConfig_Template.json and DBConfig_Template.json for necessary items.

```
cd Builds\netcoreapp3.1
mkdir Configs
cd Configs
# Create ClientConfig.json and DBConfig.json
```

### **Step. 4**

Run Avespoir

```
cd ../
dotnet Avespoir.dll
# Alpha version is AvespoirTest.Test.dll
```

## Licence
Avespoir is licensed under the Apache License 2.0. Please see the licence file for more information.

-> https://github.com/Avespoir-Project/Avespoir/blob/master/LICENSE