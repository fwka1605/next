import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pb0601CustomerGroupMasterComponent } from './pb0601-customer-group-master.component';

describe('Pb0601CustomerGroupMasterComponent', () => {
  let component: Pb0601CustomerGroupMasterComponent;
  let fixture: ComponentFixture<Pb0601CustomerGroupMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pb0601CustomerGroupMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pb0601CustomerGroupMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
