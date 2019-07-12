import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pb0401StaffMasterComponent } from './pb0401-staff-master.component';

describe('Pb0401StaffMasterComponent', () => {
  let component: Pb0401StaffMasterComponent;
  let fixture: ComponentFixture<Pb0401StaffMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pb0401StaffMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pb0401StaffMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
