import { Component, OnInit } from '@angular/core';
import faqQuestions from 'src/assets/json/faq-json.json';
import { FaqList } from '../shared/model/faq-list';

@Component({
  selector: 'app-faq',
  templateUrl: './faq.component.html',
  styleUrls: ['./faq.component.css']
})
export class FaqComponent implements OnInit {

  public faqList: FaqList[] = JSON.parse(JSON.stringify(faqQuestions)) as FaqList[];

  constructor() { }

  ngOnInit(): void {
  }

  onClickAccordion(index: number) {
    this.faqList[index].visible = !this.faqList[index].visible;
  }

}
