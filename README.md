# ABB_Backend

Here is the [link](https://orange-rock-0985d540f.azurestaticapps.net) for application.

## Main design decisions
* simplify translation format 
  1. saved translations as json file
  2. added additional metadata "Language" that makes file selfdescriptives
* Caching results
* follow CQRS pattern:
  1. Translations Azure Function is for Read operation
  2. Dictionaries Azure Function is for Write operation
* React App are hosted in Azure Static App

Note: Backend uses Azure Key Vault in order to store Azure Blob Storage Connection String

## C4Container View
Simple representation of the system. It consists of 3 C4Containers: React Application, Backend Application, Blob Storage
![](out/docs/C4Containers_VIew/C4Container%20View.png)

## Workflow sequencing diagram
It gives a brief explanation of how works the main workflow process of the system
![](out/docs/workflow_sequence/Customer%20is%20registering.png)