import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Ph0201DataMaintenanceComponent } from './ph0201-data-maintenance.component';

describe('Ph0201DataMaintenanceComponent', () => {
  let component: Ph0201DataMaintenanceComponent;
  let fixture: ComponentFixture<Ph0201DataMaintenanceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Ph0201DataMaintenanceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Ph0201DataMaintenanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
