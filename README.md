# Danske Cloud Engineer Banking Challenge

## Parameters

| Parameter   | Short | Description | Default value|
| ----------- | ----------- | ----------- | ----------- | 
| loanAmount  | l | Loan amount | 500000 |
| loanDuration   | y | Loan duration in years | 10 | 

## Configuration

Application is configured in `App.config`.

| Parameter   | Description | Default value|
| ----------- | ----------- | ----------- | 
| Locale  | Locale with which application will be used, important for numbers | en-US |
| AnnualInterestRate   | Annual interest rate | 5 | 
| AdministrationFeePercent   | Administration fee percent | 1 | 
| AdministrationFeeMin   | Administration fee min | 10000 | 
| Currency   | Currency used only for outputing information | kr. | 

## Usage

```Danske.Cebc.Console.exe -l 500000 -y 10```

## Notes 

Currently interest rate is calculated only monthly. So Compound **cannot** be changed at the moment.
