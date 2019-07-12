import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Com0404SiteErrorComponent } from './com0404-site-error.component';

describe('Com0404SiteErrorComponent', () => {
  let component: Com0404SiteErrorComponent;
  let fixture: ComponentFixture<Com0404SiteErrorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Com0404SiteErrorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Com0404SiteErrorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
