install:
	cd ShoppingCart.UI && yarn install
	cd ShoppingCart.BFF && dotnet restore

start_ui:
	cd ShoppingCart.UI && npx ng serve

cert:
	dotnet dev-certs https

start_bff:
	dotnet run --project ShoppingCart.BFF
