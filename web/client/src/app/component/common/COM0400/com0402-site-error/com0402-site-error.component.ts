import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-com0402-site-error',
  templateUrl: './com0402-site-error.component.html',
  styleUrls: ['./com0402-site-error.component.css']
})
export class Com0402SiteErrorComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit() {
    setTimeout(() => {
      this.router.navigate(['login']);
    }, 5000);
  }

}
