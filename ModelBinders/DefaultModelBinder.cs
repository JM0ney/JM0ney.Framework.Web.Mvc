//
//  Do you like this project? Do you find it helpful? Pay it forward by hiring me as a consultant!
//  https://jason-iverson.com
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JM0ney.Framework.Web.Mvc.ModelBinders {

    public class DefaultModelBinder : System.Web.Mvc.DefaultModelBinder {

        protected String PrepareValueProviderKey( String modelName, String propertyName ) {
            if ( String.IsNullOrWhiteSpace( modelName ) )
                return propertyName;
            else
                return String.Format( "{0}.{1}", modelName, propertyName );
        }

        protected String PrepareValueProviderKey(System.Web.Mvc.ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor ) {
            return this.PrepareValueProviderKey( bindingContext.ModelName, propertyDescriptor.Name );
        }

        protected override void BindProperty( ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor ) {
            String providerKey = this.PrepareValueProviderKey( bindingContext, propertyDescriptor );

            // If the datatype of this property is an Enum decorated with the Flags() attribute, we'll set the value as follows...
            if (propertyDescriptor.PropertyType.IsEnum && propertyDescriptor.Attributes.Contains(new FlagsAttribute( ) ) ) {
                Type realType = propertyDescriptor.PropertyType;
                int realValue = 0;
                String csvString = bindingContext.ValueProvider.GetValue( providerKey )?.AttemptedValue;
                String[ ] csvValues = null;

                if ( !String.IsNullOrWhiteSpace( csvString ) ) {
                    csvValues = csvString.Split( new[ ] { ',' }, StringSplitOptions.RemoveEmptyEntries );
                    if ( csvValues.Any( ) ) {
                        foreach(String str in csvValues ) {
                            int tempValue = 0;
                            if ( int.TryParse( str, out tempValue ) ) {
                                realValue += tempValue;
                            }
                            else {
                                int tempObj = ( int ) Enum.Parse( propertyDescriptor.PropertyType, str );
                                realValue += tempObj;
                            }                            
                        }
                        propertyDescriptor.SetValue( bindingContext.Model, Convert.ChangeType( realValue, realType ) );
                    }
                    else {
                        propertyDescriptor.SetValue( bindingContext.Model, Convert.ChangeType( realValue, realType ) );
                    }
                }
            }
            else {
                // Default to original BindProperty implementation
                base.BindProperty( controllerContext, bindingContext, propertyDescriptor );

                Object tempObject = propertyDescriptor.GetValue( bindingContext.Model );
                if (propertyDescriptor.PropertyType == typeof( String ) && tempObject == null ) {
                    // For String properties, rather than a null value, bind String.Empty
                    propertyDescriptor.SetValue( bindingContext.Model, String.Empty );
                }
            }
        }

    }

}
