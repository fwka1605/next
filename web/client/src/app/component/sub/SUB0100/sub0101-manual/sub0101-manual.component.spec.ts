import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Sub0101ManualComponent } from './sub0101-manual.component';

describe('Sub0101ManualComponent', () => {
  let component: Sub0101ManualComponent;
  let fixture: ComponentFixture<Sub0101ManualComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Sub0101ManualComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Sub0101ManualComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
