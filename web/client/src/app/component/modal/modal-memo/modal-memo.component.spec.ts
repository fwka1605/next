import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalMemoComponent } from './modal-memo.component';

describe('ModalReceiptMemoComponent', () => {
  let component: ModalMemoComponent;
  let fixture: ComponentFixture<ModalMemoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ModalMemoComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalMemoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
