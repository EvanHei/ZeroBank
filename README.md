# ZeroBank

A Windows desktop app for private financial transactions using homomorphic encyrption.

## Table of Contents

1. [Getting Started](#getting-started)
2. [Guide](#guide)
3. [Technologies](#technologies)
4. [Security Features](#security-features)

## Getting Started

<details>
<summary><strong>Run from executable</strong></summary>

1. Download [ZeroBank.exe](RELEASE_URL) (SHA256 below)

   ```SHA256
   <SHA256_HASH>
   ```

2. Double-click ZeroBank.exe, click "More info", and then click "Run anyway". This prompt will disappear the next time ZeroBank is run.

   <img src="./images/MoreInfo.png" width="350"><img src="./images/RunAnyway.png" width="350">

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

### Dashboard Form

<details>
<summary><strong>Summary</strong></summary>
The Dashboard Form displays records added to ZeroBank’s scope, allows the addition of data entries, provides a key generation feature, and includes log navigation.

- **Guide 📖**: opens the GitHub repository in the default browser.
- **Keys 🔑**: displays the key panel.
- **Logs 📜**: displays the log panel.
- **Add ▼**: shows dropdown options to add entries manually or import them.
- **Record List**: right click on an item to display options or drag and drop to add them.
- **Search Box**: filters entries based on query. Filter by type or tag using keywords.

<img src="./images/DashboardForm_NoRecordsPanel.png" width="1000">

</details>

---

<details>
<summary><strong>Encrypt Form</strong></summary>
<img src="./images/EncryptForm.png" width="400">

The Encrypt Form allows encryption using a selected algorithm and password. Inputs are cleared after inactivity.

- **Generate Random**: generates a compliant password.
- **Clear**: clears all password fields.
- **→**: encrypts the data. Data cannot be decrypted without the password.
- **👁**: toggle password visibility.

</details>

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
- **Unit Testing**: <UNIT_TEST_FRAMEWORK>

## Security Features

- **Confidentiality**: encryption using modern algorithms.
- **Integrity**: digital signatures and integrity checks.
- **Password Strength Policy**: ensures secure password selection.
- **Password Generation**: eliminates keystroke exposure.
- **Password Management**: clears inputs after timeout.
- **Key Derivation**: transforms passwords into keys securely.
- **Logging**: security-relevant events recorded.
- **Constant-Time Comparison**: mitigates timing attacks.
- **Secure Deletion**: overwrites data before deletion.
- **Clean Architecture**: facilitates security patches and feature updates.
