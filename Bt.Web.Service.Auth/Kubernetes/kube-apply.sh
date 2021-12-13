#!/bin/sh

set -x

# Configure kubectl

kubectl config set-cluster k8s --server="$KUBE_URL" --insecure-skip-tls-verify=true
kubectl config set-credentials currentuser --token="$KUBE_TOKEN"
kubectl config set-context default --cluster=k8s --user=currentuser
kubectl config use-context default

# Deploy service
kubectl apply -f ./deployment-dev.yaml