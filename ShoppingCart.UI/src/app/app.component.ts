import {Component, OnInit} from '@angular/core';
import {Apollo, gql} from 'apollo-angular';

type Review = {
  reviewer: string,
  content: string,
  stars: string
}

type Product = {
  name: string,
  description: string
  price: number,
  reviews: Review[]
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Products';
  products: Product[];
  loading = true;
  error: any;

  constructor(private apollo: Apollo) {
    this.products = [];
  }

  ngOnInit() {
    this.apollo
      .watchQuery({
        query: gql`
          {
            products {
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
        `,
      })
      .valueChanges.subscribe((result: any) => {
        this.products = result?.data?.products;
        this.loading = result.loading;
        this.error = result.error;
      });
  }
}
