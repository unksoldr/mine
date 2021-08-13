﻿using System;
using System.Linq.Expressions;
using AOSharp.Common.GameData;
using SmokeLounge.AOtomation.Messaging.GameData;
using SmokeLounge.AOtomation.Messaging.Messages;
using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

namespace SmokeLounge.AOtomation.Messaging.Serialization.Serializers.Custom
{
    class PlayfieldTowerUpdateClientSerializer : ISerializer
    {
        public Type Type { get; }

        public object Deserialize(StreamReader streamReader, SerializationContext serializationContext, PropertyMetaData propertyMetaData = null)
        {
            PlayfieldTowerUpdateClientMessage updateMessage = new PlayfieldTowerUpdateClientMessage();
            updateMessage.N3MessageType = (N3MessageType)streamReader.ReadInt32();
            updateMessage.Identity = new Identity((IdentityType)streamReader.ReadInt32(), streamReader.ReadInt32());
            updateMessage.Unknown = streamReader.ReadByte();
            updateMessage.TowerId = new Identity((IdentityType)streamReader.ReadInt32(), streamReader.ReadInt32());
            updateMessage.UpdateType = (PlayfieldUpdateClientType)streamReader.ReadInt32();

            if (updateMessage.UpdateType == PlayfieldUpdateClientType.Planted)
            {
                updateMessage.Tower = new DummyTower
                {
                    Identity = new Identity((IdentityType)streamReader.ReadInt32(), streamReader.ReadInt32()),
                    CharIdentity = new Identity((IdentityType)streamReader.ReadInt32(), streamReader.ReadInt32()),
                    Position = new Vector3(streamReader.ReadSingle(), streamReader.ReadSingle(), streamReader.ReadSingle()),
                    MeshId = streamReader.ReadInt32(),
                    Side = (Side)streamReader.ReadInt32(),
                    DestroyedMeshId = streamReader.ReadInt32(),
                    Scale = streamReader.ReadSingle(),
                    Class = (TowerClass)streamReader.ReadInt32()
                };
            }

            return updateMessage;
        }

        public Expression DeserializerExpression(ParameterExpression streamReaderExpression,
            ParameterExpression serializationContextExpression, Expression assignmentTargetExpression,
            PropertyMetaData propertyMetaData)
        {
            var deserializerMethodInfo =
                ReflectionHelper
                    .GetMethodInfo
                        <GenericCmdSerializer, Func<StreamReader, SerializationContext, PropertyMetaData, object>>
                        (o => o.Deserialize);
            var serializerExp = Expression.New(this.GetType());
            var callExp = Expression.Call(
                serializerExp,
                deserializerMethodInfo,
                new Expression[]
                {
                    streamReaderExpression, serializationContextExpression,
                    Expression.Constant(propertyMetaData, typeof(PropertyMetaData))
                });

            var assignmentExp = Expression.Assign(
                assignmentTargetExpression, Expression.TypeAs(callExp, assignmentTargetExpression.Type));
            return assignmentExp;
        }

        public void Serialize(StreamWriter streamWriter, SerializationContext serializationContext, object value, PropertyMetaData propertyMetaData = null)
        {
            throw new NotImplementedException();
        }

        public Expression SerializerExpression(ParameterExpression streamWriterExpression,
            ParameterExpression serializationContextExpression, Expression valueExpression, PropertyMetaData propertyMetaData)
        {
            var serializerMethodInfo =
                ReflectionHelper
                    .GetMethodInfo
                    <GenericCmdSerializer,
                        Action<StreamWriter, SerializationContext, object, PropertyMetaData>>(o => o.Serialize);
            var serializerExp = Expression.New(this.GetType());
            var callExp = Expression.Call(
                serializerExp,
                serializerMethodInfo,
                new[]
                {
                    streamWriterExpression, serializationContextExpression, valueExpression,
                    Expression.Constant(propertyMetaData, typeof(PropertyMetaData))
                });
            return callExp;
        }
    }
}
