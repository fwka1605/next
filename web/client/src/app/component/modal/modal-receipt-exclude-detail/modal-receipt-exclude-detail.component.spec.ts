import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalReceiptExcludeDetailComponent } from './modal-receipt-exclude-detail.component';

describe('ModalReceiptExcludeDetailComponent', () => {
  let component: ModalReceiptExcludeDetailComponent;
  let fixture: ComponentFixture<ModalReceiptExcludeDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalReceiptExcludeDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalReceiptExcludeDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
