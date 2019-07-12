import { FormControl } from "@angular/forms";

export class FormatterUtil {

  public static setNumberFormatter(formControl: FormControl) {
    formControl.valueChanges.subscribe(
      (value: string) => {
        if (value.match(/[^0-9,]/)) {
          formControl.setValue(value.replace(/[^0-9,]/g, ''));
        }
      });
  }
}