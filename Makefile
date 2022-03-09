install:
	cd ShoppingCart.UI && yarn install
	cd ShoppingCart.BFF && dotnet restore

start_web:
	cd ShoppingCart.UI && npx ng serve

cert:
	dotnet dev-certs https

start_backend:
	dotnet run --project ShoppingCart.BFF

test_backend:
	dotnet test
