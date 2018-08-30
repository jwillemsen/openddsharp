//------------------------------------------------------------------------------
// <auto-generated>
// WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.
// DDS version: 3.13
// ACE version: 6.2a_p15
// Running on input file: ShapeType.idl
// </auto-generated>
//------------------------------------------------------------------------------

#pragma once

#pragma unmanaged
#include <dds/DCPS/Service_Participant.h>
#include "ShapeTypeTypeSupportImpl.h"
#pragma managed

#include <vcclr.h>
#include <msclr/marshal.h>
#include "LNK4248.h"

using namespace System::Collections::Generic;

// Incomplete types generate LNK4248 warnings when compiled for .NET
SUPPRESS_LNK4248_CORBA

namespace OpenDDSharp {
namespace org {
namespace omg {
namespace dds {
namespace demo {

    public ref class ShapeType {

    private:
        System::String^ m_color;
        System::Int32 m_x;
        System::Int32 m_y;
        System::Int32 m_shapesize;

    public:
        property System::String^ color {
            System::String^ get();
            void set(System::String^ value);
        }
        property System::Int32 x {
            System::Int32 get();
            void set(System::Int32 value);
        }
        property System::Int32 y {
            System::Int32 get();
            void set(System::Int32 value);
        }
        property System::Int32 shapesize {
            System::Int32 get();
            void set(System::Int32 value);
        }

    public:
        ShapeType();

    internal:
        ::org::omg::dds::demo::ShapeType ToNative();
        void FromNative(::org::omg::dds::demo::ShapeType native);
    };

    public ref class ShapeTypeTypeSupport {

	private:
		msclr::interop::marshal_context context;
        static ::org::omg::dds::demo::ShapeTypeTypeSupport_ptr impl_entity;

	public:
		ShapeTypeTypeSupport();
		System::String^ GetTypeName();
		OpenDDSharp::DDS::ReturnCode RegisterType(::OpenDDSharp::DDS::DomainParticipant^ participant, System::String^ typeName);
        OpenDDSharp::DDS::ReturnCode UnregisterType(::OpenDDSharp::DDS::DomainParticipant^ participant, System::String^ typeName);

	};

///////////////////////////////////////////////////////////////////////

	public ref class ShapeTypeDataWriter : OpenDDSharp::DDS::DataWriter {

	private:
		::org::omg::dds::demo::ShapeTypeDataWriter_ptr impl_entity;

	public:
		ShapeTypeDataWriter(::OpenDDSharp::DDS::DataWriter^ dataWriter);
        OpenDDSharp::DDS::InstanceHandle RegisterInstance(ShapeType^ instance);
        OpenDDSharp::DDS::InstanceHandle RegisterInstance(ShapeType^ instance, OpenDDSharp::DDS::Timestamp timestamp);
        OpenDDSharp::DDS::ReturnCode UnregisterInstance(ShapeType^ data);
		OpenDDSharp::DDS::ReturnCode UnregisterInstance(ShapeType^ data, OpenDDSharp::DDS::InstanceHandle handle);
        OpenDDSharp::DDS::ReturnCode UnregisterInstance(ShapeType^ data,OpenDDSharp::DDS::InstanceHandle handle, OpenDDSharp::DDS::Timestamp timestamp);
		OpenDDSharp::DDS::ReturnCode Write(ShapeType^ data);
        OpenDDSharp::DDS::ReturnCode Write(ShapeType^ data, OpenDDSharp::DDS::InstanceHandle handle);
        OpenDDSharp::DDS::ReturnCode Write(ShapeType^ data, OpenDDSharp::DDS::InstanceHandle handle, OpenDDSharp::DDS::Timestamp timestamp);
		OpenDDSharp::DDS::ReturnCode Delete(ShapeType^ data);
		OpenDDSharp::DDS::ReturnCode Delete(ShapeType^ data, OpenDDSharp::DDS::InstanceHandle handle);
        OpenDDSharp::DDS::ReturnCode Delete(ShapeType^ data, OpenDDSharp::DDS::InstanceHandle handle, OpenDDSharp::DDS::Timestamp timestamp);
        OpenDDSharp::DDS::ReturnCode GetKeyValue(ShapeType^ data, OpenDDSharp::DDS::InstanceHandle handle);
        System::Int32 LookupInstance(ShapeType^ instance);
	};

///////////////////////////////////////////////////////////////////////

	public ref class ShapeTypeDataReader : OpenDDSharp::DDS::DataReader {

	private:
		::org::omg::dds::demo::ShapeTypeDataReader_ptr impl_entity;

	public:
		ShapeTypeDataReader(::OpenDDSharp::DDS::DataReader^ dataReader);

        OpenDDSharp::DDS::ReturnCode Read(List<ShapeType^>^ receivedData,
										  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo);

		OpenDDSharp::DDS::ReturnCode Read(List<ShapeType^>^ receivedData,
										  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
										  System::Int32 maxSamples);

        OpenDDSharp::DDS::ReturnCode Read(List<ShapeType^>^ receivedData,
										  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
										  System::Int32 maxSamples,
										  OpenDDSharp::DDS::ReadCondition^ condition);

        OpenDDSharp::DDS::ReturnCode Read(List<ShapeType^>^ receivedData,
										  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
										  System::Int32 maxSamples,
										  OpenDDSharp::DDS::SampleStateMask sampleStates,
										  OpenDDSharp::DDS::ViewStateMask viewStates,
										  OpenDDSharp::DDS::InstanceStateMask instanceStates);

        OpenDDSharp::DDS::ReturnCode Take(List<ShapeType^>^ receivedData,
										  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo);

		OpenDDSharp::DDS::ReturnCode Take(List<ShapeType^>^ receivedData,
										  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
										  System::Int32 maxSamples);

        OpenDDSharp::DDS::ReturnCode Take(List<ShapeType^>^ receivedData,
										  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
										  System::Int32 maxSamples,
										  OpenDDSharp::DDS::ReadCondition^ condition);        

        OpenDDSharp::DDS::ReturnCode Take(List<ShapeType^>^ receivedData,
										  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
										  System::Int32 maxSamples,
										  OpenDDSharp::DDS::SampleStateMask sampleStates,
										  OpenDDSharp::DDS::ViewStateMask viewStates,
										  OpenDDSharp::DDS::InstanceStateMask instanceStates);
        
        OpenDDSharp::DDS::ReturnCode ReadInstance(List<ShapeType^>^ receivedData,
												  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
												  OpenDDSharp::DDS::InstanceHandle handle);

		OpenDDSharp::DDS::ReturnCode ReadInstance(List<ShapeType^>^ receivedData,
												  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
												  OpenDDSharp::DDS::InstanceHandle handle,
												  System::Int32 maxSamples);

		OpenDDSharp::DDS::ReturnCode ReadInstance(List<ShapeType^>^ receivedData,
												  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
												  OpenDDSharp::DDS::InstanceHandle handle,
												  System::Int32 maxSamples,
												  OpenDDSharp::DDS::ReadCondition^ condition);

        OpenDDSharp::DDS::ReturnCode ReadInstance(List<ShapeType^>^ receivedData,
												  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,												  
												  OpenDDSharp::DDS::InstanceHandle handle,
                                                  System::Int32 maxSamples,
												  OpenDDSharp::DDS::SampleStateMask sampleStates,
												  OpenDDSharp::DDS::ViewStateMask viewStates,
												  OpenDDSharp::DDS::InstanceStateMask instanceStates);

        OpenDDSharp::DDS::ReturnCode TakeInstance(List<ShapeType^>^ receivedData,
												  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
												  OpenDDSharp::DDS::InstanceHandle handle);

		OpenDDSharp::DDS::ReturnCode TakeInstance(List<ShapeType^>^ receivedData,
												  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
												  OpenDDSharp::DDS::InstanceHandle handle,
												  System::Int32 maxSamples);

		OpenDDSharp::DDS::ReturnCode TakeInstance(List<ShapeType^>^ receivedData,
												  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
												  OpenDDSharp::DDS::InstanceHandle handle,
												  System::Int32 maxSamples,
												  OpenDDSharp::DDS::ReadCondition^ condition);

        OpenDDSharp::DDS::ReturnCode TakeInstance(List<ShapeType^>^ receivedData,
												  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
												  OpenDDSharp::DDS::InstanceHandle handle,
												  System::Int32 maxSamples,
												  OpenDDSharp::DDS::SampleStateMask sampleStates,
												  OpenDDSharp::DDS::ViewStateMask viewStates,
												  OpenDDSharp::DDS::InstanceStateMask instanceStates);

        OpenDDSharp::DDS::ReturnCode ReadNextInstance(List<ShapeType^>^ receivedData,
												      List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
												      OpenDDSharp::DDS::InstanceHandle handle);

		OpenDDSharp::DDS::ReturnCode ReadNextInstance(List<ShapeType^>^ receivedData,
												      List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
												      OpenDDSharp::DDS::InstanceHandle handle,
												      System::Int32 maxSamples);

		OpenDDSharp::DDS::ReturnCode ReadNextInstance(List<ShapeType^>^ receivedData,
												      List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
												      OpenDDSharp::DDS::InstanceHandle handle,
												      System::Int32 maxSamples,
												      OpenDDSharp::DDS::ReadCondition^ condition);

        OpenDDSharp::DDS::ReturnCode ReadNextInstance(List<ShapeType^>^ receivedData,
												      List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
												      OpenDDSharp::DDS::InstanceHandle handle,
												      System::Int32 maxSamples,
												      OpenDDSharp::DDS::SampleStateMask sampleStates,
												      OpenDDSharp::DDS::ViewStateMask viewStates,
												      OpenDDSharp::DDS::InstanceStateMask instanceStates);

         OpenDDSharp::DDS::ReturnCode TakeNextInstance(List<ShapeType^>^ receivedData,
												       List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
												       OpenDDSharp::DDS::InstanceHandle handle);

		OpenDDSharp::DDS::ReturnCode TakeNextInstance(List<ShapeType^>^ receivedData,
												      List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
												      OpenDDSharp::DDS::InstanceHandle handle,
												      System::Int32 maxSamples);

		OpenDDSharp::DDS::ReturnCode TakeNextInstance(List<ShapeType^>^ receivedData,
												      List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
												      OpenDDSharp::DDS::InstanceHandle handle,
												      System::Int32 maxSamples,
												      OpenDDSharp::DDS::ReadCondition^ condition);

        OpenDDSharp::DDS::ReturnCode TakeNextInstance(List<ShapeType^>^ receivedData,
												      List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
												      OpenDDSharp::DDS::InstanceHandle handle,
												      System::Int32 maxSamples,
												      OpenDDSharp::DDS::SampleStateMask sampleStates,
												      OpenDDSharp::DDS::ViewStateMask viewStates,
												      OpenDDSharp::DDS::InstanceStateMask instanceStates);

		OpenDDSharp::DDS::ReturnCode ReadNextSample(ShapeType^ data, ::OpenDDSharp::DDS::SampleInfo^ sampleInfo);

		OpenDDSharp::DDS::ReturnCode TakeNextSample(ShapeType^ data, ::OpenDDSharp::DDS::SampleInfo^ sampleInfo);

        OpenDDSharp::DDS::ReturnCode GetKeyValue(ShapeType^ data, OpenDDSharp::DDS::InstanceHandle handle);

        OpenDDSharp::DDS::InstanceHandle LookupInstance(ShapeType^ instance);
	};

///////////////////////////////////////////////////////////////////////
};
};
};
};
};
