import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-quick-links',
  templateUrl: './quick-links.component.html',
  styleUrls: ['./quick-links.component.css']
})
export class QuickLinksComponent implements OnInit {

  public selectedLink: string;
  constructor(private router: Router) {
    this.selectedLink = router.url;
  }

  ngOnInit(): void {
  }

}
