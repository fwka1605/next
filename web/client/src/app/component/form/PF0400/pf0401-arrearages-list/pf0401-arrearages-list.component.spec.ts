import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pf0401ArrearagesListComponent } from './pf0401-arrearages-list.component';

describe('Pf0401ArrearagesListComponent', () => {
  let component: Pf0401ArrearagesListComponent;
  let fixture: ComponentFixture<Pf0401ArrearagesListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pf0401ArrearagesListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pf0401ArrearagesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
