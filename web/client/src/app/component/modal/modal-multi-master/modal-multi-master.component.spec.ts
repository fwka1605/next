import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalMultiMasterComponent } from './modal-multi-master.component';

describe('ModalMultiMasterComponent', () => {
  let component: ModalMultiMasterComponent;
  let fixture: ComponentFixture<ModalMultiMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalMultiMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalMultiMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
