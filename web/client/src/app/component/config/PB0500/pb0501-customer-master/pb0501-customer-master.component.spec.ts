import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pb0501CustomerMasterComponent } from './pb0501-customer-master.component';

describe('Pb0501CustomerMasterComponent', () => {
  let component: Pb0501CustomerMasterComponent;
  let fixture: ComponentFixture<Pb0501CustomerMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pb0501CustomerMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pb0501CustomerMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
