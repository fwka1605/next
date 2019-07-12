import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pf0601CollectionScheduleComponent } from './pf0601-collection-schedule.component';

describe('Pf0601CollectionScheduleComponent', () => {
  let component: Pf0601CollectionScheduleComponent;
  let fixture: ComponentFixture<Pf0601CollectionScheduleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pf0601CollectionScheduleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pf0601CollectionScheduleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
