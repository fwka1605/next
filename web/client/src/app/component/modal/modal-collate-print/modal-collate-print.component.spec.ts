import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalCollatePrintComponent } from './modal-collate-print.component';

describe('ModalCollatePrintAtComponent', () => {
  let component: ModalCollatePrintComponent;
  let fixture: ComponentFixture<ModalCollatePrintComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalCollatePrintComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalCollatePrintComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
