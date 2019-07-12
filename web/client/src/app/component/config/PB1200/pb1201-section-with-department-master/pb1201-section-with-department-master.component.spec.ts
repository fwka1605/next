import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pb1201SectionWithDepartmentMasterComponent } from './pb1201-section-with-department-master.component';

describe('Pb1201SectionWithDepartmentMasterComponent', () => {
  let component: Pb1201SectionWithDepartmentMasterComponent;
  let fixture: ComponentFixture<Pb1201SectionWithDepartmentMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pb1201SectionWithDepartmentMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pb1201SectionWithDepartmentMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
