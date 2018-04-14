#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsTopicC.h>
#pragma managed

#include "TopicDescription.h"
#include "DomainParticipant.h"
#include "ViewStateKind.h"
#include "InstanceStateKind.h"

#include <vcclr.h>
#include <msclr/marshal.h>

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// MultiTopic is an implementation of <see cref="ITopicDescription" /> that allows subscriptions to combine/filter/rearrange data coming from
		/// several topics. MultiTopic allows a more sophisticated subscription that can select and combine data received from multiple topics into a
		/// single resulting type(specified by the inherited type name). The data will then be filtered(selection) and possibly re-arranged
		/// (aggregation/projection) according to a subscription expression with the expression parameters.
		/// </summary>
		/// <remarks>
		/// <p>The subscription expression is a string that identifies the selection and re-arrangement of data from the associated
		/// topics. It is similar to an SQL clause where the SELECT part provides the fields to be kept, the FROM part provides the
		///	names of the topics that are searched for those fields, and the WHERE clause gives the content filter.The topics
		///	combined may have different types but they are restricted in that the type of the fields used for the NATURAL JOIN
		///	operation must be the same.</p>
		/// <p>The expression parameters are a collection of strings that give values to the ‘parameters’ ("%n" tokens) in
		/// the subscription expression. The number of supplied parameters must fit with the requested values in the
		///	subscription expression (the number of %n tokens).</p>
		/// <p><see cref="DataReader" /> entities associated with a MultiTopic are alerted of data modifications by the usual listener or condition
		/// mechanisms whenever modifications occur to the data associated with any of the topics relevant to the MultiTopic.</p>
		/// <p><see cref="DataReader" /> entities associated with a MultiTopic access instances that are “constructed” at the <see cref="DataReader" /> side from
		/// the instances written by multiple <see cref="DataWriter" /> entities.The MultiTopic access instance will begin to exist as soon as all
		///	the constituting Topic instances are in existence.</p>
		/// <p>The view_state and instance_state is computed from the corresponding states of the constituting instances:</p>
		/// <list type="bullet">
		///		<item><description>The view state of the MultiTopic instance is <see cref="ViewStateKind::NewViewState" /> if at least one of the constituting instances has
		/// <see cref="ViewStateKind::NewViewState" />, otherwise it will be  <see cref="ViewStateKind::NotNewViewState" />.</description></item>
		///		<item><description>The instance state of the MultiTopic instance is <see cref="InstanceStateKind::AliveInstanceState" /> if the instance state of all the
		/// constituting Topic instances is <see cref="InstanceStateKind::AliveInstanceState" />. It is <see cref="InstanceStateKind::NotAliveDisposedInstanceState" /> if at least one of the constituting <see cref="Topic" />
		///	instances is <see cref="InstanceStateKind::NotAliveDisposedInstanceState" />. Otherwise it is <see cref="InstanceStateKind::NotAliveNoWritersInstanceState" />.</description></item>
		///	</list>
		/// </remarks>
		public ref class MultiTopic : public OpenDDSharp::DDS::TopicDescription {

		internal:
			::DDS::MultiTopic_ptr impl_entity;

		public:
			/// <summary>
			/// Gets the subscription expression associated with the <see cref="MultiTopic" />. 
			/// That is, the expression specified when the <see cref="MultiTopic" /> was created.
			/// </summary>
			property System::String^ SubscriptionExpression {
				System::String^ get();
			}

		internal:
			MultiTopic(::DDS::MultiTopic_ptr native);

		public:
			/// <summary>
			/// Gets the expression parameters associated with the <see cref="MultiTopic" />. That is, the parameters specified on the last
			/// successful call to <see cref="MultiTopic::SetExpressionParameters" />, or if it was never called, the parameters specified
			///	when the <see cref="MultiTopic" /> was created.
			/// </summary>
			/// <param name="params">The expression parameters collection to be filled up.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetExpressionParameters(ICollection<System::String^>^ params);

			/// <summary>
			/// Changes the expression parameters associated with the <see cref="MultiTopic" />.
			/// </summary>
			/// <param name="params">The expression parameters collection to be set.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode SetExpressionParameters(ICollection<System::String^>^ params);

		private:
			System::String^ GetSubscriptionExpression();
		};
	};
};
