# Angular C\# GraphQL Example

> Example project for setting up a GraphQL powered Angular app using .NET Core 6.

## Requirements

- [GNU Make](https://www.gnu.org/software/make/)
- [.NET Core 6](https://dotnet.microsoft.com/)
- [Angular](https://angular.io)

## Usage

To install project dependencies, run the following command:
```bash
make install
```

To run the `ui` project, run the following command:
```bash
make start_ui
```

To run the `bff` project, run the following command:
```bash
make start_bff
```

## Sample Queries & Mutations

```grapql
{
  products {
    id
    name
    description
    reviews {
      reviewer
      content
      stars
    }
  }
}

mutation AddProduct($name: String!, $price: Decimal!, $description: String!) {
  addProduct (name: $name, price: $price, description: $description) {
    name
    price
    description
  }
}

mutation AddReviewForProduct($id: ID!, $review: ReviewInput!) {
  addReview(id: $id, review: $review) {
    name
    description
    price
    
    reviews {
      reviewer
      content
      stars
    }
  }
}
```

## Links

- [Very useful .NET GraphQL link](https://dev.to/berviantoleo/getting-started-graphql-in-net-6-part-1-4ic2)
- [Another very useful .NET GraphQL link](https://www.red-gate.com/simple-talk/development/dotnet-development/building-and-consuming-graphql-api-in-asp-net-core-5/)
- [Mutations guide](https://graphql.org/learn/queries/#mutations)
