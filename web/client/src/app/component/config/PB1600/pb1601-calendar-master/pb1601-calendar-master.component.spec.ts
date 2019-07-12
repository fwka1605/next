import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pb1601CalendarMasterComponent } from './pb1601-calendar-master.component';

describe('Pb1601CalendarMasterComponent', () => {
  let component: Pb1601CalendarMasterComponent;
  let fixture: ComponentFixture<Pb1601CalendarMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pb1601CalendarMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pb1601CalendarMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
