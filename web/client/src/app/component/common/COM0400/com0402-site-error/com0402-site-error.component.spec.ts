import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Com0402SiteErrorComponent } from './com0402-site-error.component';

describe('Com0402SiteErrorComponent', () => {
  let component: Com0402SiteErrorComponent;
  let fixture: ComponentFixture<Com0402SiteErrorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Com0402SiteErrorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Com0402SiteErrorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
