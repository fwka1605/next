import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalConfirmMatchingComponent } from './modal-confirm-matching.component';

describe('ModalConfirmMatchingComponent', () => {
  let component: ModalConfirmMatchingComponent;
  let fixture: ComponentFixture<ModalConfirmMatchingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalConfirmMatchingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalConfirmMatchingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
