import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalReceiptExcludeAllComponent } from './modal-receipt-exclude-all.component';

describe('ModalReceiptExcludeAllComponent', () => {
  let component: ModalReceiptExcludeAllComponent;
  let fixture: ComponentFixture<ModalReceiptExcludeAllComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalReceiptExcludeAllComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalReceiptExcludeAllComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
