# Budget

Budget is a simple budget manager written in C# using Xamarin.Forms. It allows you to create wallets and track income and outgoing.


It's an Android application composed of three pages: movements, wallets and settings.

It connects to a local SQLite database to get data.


# Movements page


After creating a wallet, movements can be added to it clicking the button in the movements page.

A movement has a description, an amount, a date and a type, which can be incoming, outgoing or draft.

The total of income and outgoing is calculated at the end of the table and added to the initial balance. Draft movements are ignored during calculation.


![](https://github.com/carloesposiito/Budget-Android-App/blob/main/IMGs/Movements%20page_.png)


The button present on each movement opens a new page where it is possible to edit the movement, confirm it on the starting balance or cancel it.


![](https://github.com/carloesposiito/Budget-Android-App/blob/main/IMGs/Edit%20movement%20page_.png)


# Wallets page


Wallets can be created clicking the button in the wallets page.


![](https://github.com/carloesposiito/Budget-Android-App/blob/main/IMGs/Wallets%20page_.png)


For each wallet displayed there is a button that opens a new page where they can be modified or deleted.


![](https://github.com/carloesposiito/Budget-Android-App/blob/main/IMGs/Edit%20wallet%20page_.png)


# Settings page


Wallets can be made visible or not in the movements page clicking the button near their name.

Info button opens a new page with contacts of Budget developer.


![](https://github.com/carloesposiito/Budget-Android-App/blob/main/IMGs/Settings%20page_.png)

