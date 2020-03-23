# Avespoir

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
git clone https://gitlab.com/Avespoir_Project/Avespoir.git 
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

-> https://gitlab.com/Avespoir_Project/Avespoir/-/blob/master/LICENSE