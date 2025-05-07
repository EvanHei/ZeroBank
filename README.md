@ -1,105 +1,2 @@

# ZeroBank

A Windows desktop app for private financial transactions using homomorphic encyrption.

## Table of Contents

1. [Getting Started](#getting-started)
2. [Guide](#guide)
3. [Technologies](#technologies)

## Getting Started

<details>
<summary><strong>Run from executable</strong></summary>

1. Download [ZeroBank.exe](RELEASE_URL) (SHA256 below)

   ```SHA256
   <SHA256_HASH>
   ```

2. Double-click ZeroBank.exe, click "More info", and then click "Run anyway". This prompt will disappear the next time ZeroBank is run.

</details>

<details>
<summary><strong>Run from source code</strong></summary>

1. Download the .NET SDK from Microsoft's website <a href="https://dotnet.microsoft.com/download"> here</a> or verify installation by running the following command:

   ```bash
   dotnet --version
   ```

2. Navigate to `<SOURCE_DIRECTORY>` and launch with the following command:

   ```bash
   dotnet run
   ```

</details>

## Guide

### Dashboard Tab

The Dashboard tab displays...

- **User Guide**: opens this README in a new browser tab.
- **Create Account**: opens the account creation dialog.

   <img src="./images/ClientDashboard.png" width=800>

### Accounts Tab

The Accounts Form tab...

- **Create New**: opens the account creation dialog.
- **Delete**: deletes an account if it has no transactions.

   <img src="./images/ClientAccounts.png" width=800>

---

## Technologies

- **OS**: Windows
- **IDE**: Visual Studio
- **Programming Language**: C#
- **Framework**: .NET
- **UI**: Windows Forms
- **Version Control**: Git / GitHub
- **Algorithms**: <ALGORITHMS_USED>
- **Logging**: <LOGGING_LIBRARY>
- **Unit Testing**: xUnit
