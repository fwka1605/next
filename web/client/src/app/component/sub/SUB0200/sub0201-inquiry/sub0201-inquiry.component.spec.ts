import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Sub0201InquiryComponent } from './sub0201-inquiry.component';

describe('Sub0201InquiryComponent', () => {
  let component: Sub0201InquiryComponent;
  let fixture: ComponentFixture<Sub0201InquiryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Sub0201InquiryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Sub0201InquiryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
