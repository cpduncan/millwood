using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[UxmlObject]
public partial class CustomInputDisplayBinding : CustomBinding
{

    public enum Device
    {
        Keyboard,
        Gamepad
    }

    [UxmlAttribute] public Device device;

    public CustomInputDisplayBinding()
    {
        updateTrigger = BindingUpdateTrigger.OnSourceChanged;
    }

    protected override void OnDataSourceChanged(in DataSourceContextChanged context)
    {
        
        //Get the target element
        VisualElement element = context.targetElement;
        
        //Get data source from target element
        object data = element.dataSource;
        if (element.dataSource == null)
        {
            DataSourceContext parentContext = element.GetHierarchicalDataSourceContext();
            data = parentContext.dataSource;
        }

        if (data != null)
        {
            if (data is InputAction inputAction)
            {
                
                
                // Debug.Log($"Action: {inputAction.name}");
                //
                // foreach (var b in inputAction.bindings)
                // {
                //     Debug.Log(
                //         $"Binding: '{b.name}' " +
                //         $"groups='{b.groups}' " +
                //         $"path='{b.path}' " +
                //         $"effectivePath='{b.effectivePath}' " +
                //         $"isComposite={b.isComposite} " +
                //         $"isPart={b.isPartOfComposite}");
                // }        
        
                
                //find binding for device type
                InputBinding binding = GetInputBindingForDevice(device, inputAction);
                string value = "";
                if (binding.isComposite)
                {
                    value = binding.name;
                }
                else
                {
                    value = binding.ToDisplayString();
                }
                //If it's composite, just use the name
                ConverterGroups.TrySetValueGlobal(ref element, context.bindingId, value, out var errorCode);
            }
        }
    }

    private InputBinding GetInputBindingForDevice(Device device, InputAction inputAction)
    {
        // foreach (InputBinding binding in inputAction.bindings)
        // {
        //     if (binding.isPartOfComposite)
        //     {
        //         continue;
        //     }
        //     if (device == Device.Keyboard && binding.isComposite && binding.path.Contains("Dpad"))
        //     {
        //         return binding;
        //     }
        //     if (device == Device.Keyboard && binding.groups.Contains("Keyboard"))
        //     {
        //         return binding;
        //     }
        //     if (device == Device.Gamepad && binding.groups.Contains("Gamepad"))
        //     {
        //         return binding;
        //     }
        // }
        // return new InputBinding();

        foreach (var binding in inputAction.bindings)
        {
            if (binding.isPartOfComposite)
                continue;

            switch (device)
            {
                case Device.Keyboard:
                    if (binding.isComposite)
                        return binding;

                    if (binding.effectivePath.StartsWith("<Keyboard>") ||
                        binding.effectivePath.StartsWith("<Mouse>"))
                        return binding;
                    break;

                case Device.Gamepad:
                    if (binding.effectivePath.StartsWith("<Gamepad>"))
                        return binding;
                    break;
            }
        }

        return default;
    }
}
