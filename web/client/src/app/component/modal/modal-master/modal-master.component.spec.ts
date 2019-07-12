import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalMasterComponent } from './modal-master.component';

describe('ModalMasterComponent', () => {
  let component: ModalMasterComponent;
  let fixture: ComponentFixture<ModalMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
