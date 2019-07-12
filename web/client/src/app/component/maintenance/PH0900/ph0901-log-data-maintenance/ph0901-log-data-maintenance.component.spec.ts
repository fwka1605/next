import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Ph0901LogDataMaintenanceComponent } from './ph0901-log-data-maintenance.component';

describe('Ph0901LogDataMaintenanceComponent', () => {
  let component: Ph0901LogDataMaintenanceComponent;
  let fixture: ComponentFixture<Ph0901LogDataMaintenanceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Ph0901LogDataMaintenanceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Ph0901LogDataMaintenanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
