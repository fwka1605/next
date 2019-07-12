import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pb0201DepartmentMasterComponent } from './pb0201-department-master.component';

describe('Pb0201DepartmentMasterComponent', () => {
  let component: Pb0201DepartmentMasterComponent;
  let fixture: ComponentFixture<Pb0201DepartmentMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pb0201DepartmentMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pb0201DepartmentMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
