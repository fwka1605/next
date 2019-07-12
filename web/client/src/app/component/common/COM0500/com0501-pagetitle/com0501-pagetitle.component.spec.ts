import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Com0501PageTitleComponent } from './com0501-pagetitle.component';

describe('Com0501PageTitleComponent', () => {
  let component: Com0501PageTitleComponent;
  let fixture: ComponentFixture<Com0501PageTitleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Com0501PageTitleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Com0501PageTitleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
