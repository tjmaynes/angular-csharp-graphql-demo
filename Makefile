install:
	cd ShoppingCart.UI && yarn install
	cd ShoppingCart.BFF && dotnet restore

start_web:
	cd ShoppingCart.UI && ng run

start_backend:
	dotnet run --project ShoppingCart.BFF
